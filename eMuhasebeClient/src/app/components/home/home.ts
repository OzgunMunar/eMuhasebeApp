import { AfterViewInit, Component } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { initialPurchaseReportModel, PurchaseReportModel } from '../../models/purchasereportmodels/purchasereport.model';
import { HttpService } from '../../services/http.service';
import { SignalrService } from '../../services/signalr';
import { DatePipe } from '@angular/common';
declare const Chart: any;

@Component({

  selector: 'app-home',
  imports: [
    SharedModule
  ],
  templateUrl: './home.html',
  styleUrl: './home.css',
  providers: [DatePipe]

})

export default class Home implements AfterViewInit {

  constructor(
    private http: HttpService,
    private signalR: SignalrService,
    private date: DatePipe
  ) { }

  chart: any
  response: PurchaseReportModel = { ...initialPurchaseReportModel }

  ngAfterViewInit(): void {

    this.showChart()
    this.getPurchaseReports()

    this.signalR.connect(() => {

      this.signalR.hub?.on("PurchaseReports", (res: { date: string, amount: number }) => {

        if (this.response.dates.find(p => p == res.date)) {

          const index = this.response.dates.findIndex(p => p == res.date)
          this.response.amounts[index] += res.amount

        } else {

          this.response.dates.push(res.date)
          this.response.amounts.push(res.amount)

        }

        this.response.dates = this.response.dates.sort((a, b) => {
          return a.localeCompare(b);
        });

        this.updateChart()

      })

    })

  }

  showChart() {

    const ctx = document.getElementById('myChart')

    this.chart = new Chart(ctx, {
      type: 'line',
      data: {
        labels: [],
        datasets: [{
          label: 'Günlük Satış Faturaları',
          data: [],
          borderWidth: 1
        }]
      },
      options: {
        scales: {
          y: {
            beginAtZero: true
          }
        }
      }
    })

  }

  getPurchaseReports() {

    this.http.get<PurchaseReportModel>("Reports/PurchaseReports", (res) => {

      this.response = res
      this.updateChart()

    })

  }

  updateChart() {

    const formattedDates = this.response.dates.map(d => this.date.transform(d, 'dd.MM.yyyy'))
    this.chart.data.labels = formattedDates
    this.chart.data.datasets[0].data = [...this.response.amounts]
    this.chart.update()

  }


}

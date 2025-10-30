import { AfterViewInit, Component } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { PurchaseReportModel } from '../../models/purchasereportmodels/purchasereport.model';
import { HttpService } from '../../services/http.service';
declare const Chart:any;

@Component({

  selector: 'app-home',
  imports: [
    SharedModule
  ],
  templateUrl: './home.html',
  styleUrl: './home.css'

})

export default class Home implements AfterViewInit{

  constructor(
    private http: HttpService
  ) {}

  chart: any

  ngAfterViewInit(): void {
    this.showChart()
    this.getPurchaseReports()
  }

  showChart() {

    const ctx = document.getElementById('myChart')

    this.chart = new Chart(ctx, {
      type: 'line',
      data: {
        labels: [],
        datasets: [{
          label: 'Aylara göre satış faturaları',
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

      this.chart.data.labels = res.months;
      this.chart.data.datasets[0].data = res.amounts

      this.chart.update()

    })

  }

}

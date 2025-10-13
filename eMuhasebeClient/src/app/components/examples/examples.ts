import { Component, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { SharedModule } from '../../modules/shared.module';
import { ExamplePipe } from '../../pipes/example-pipe';
import { ExampleModel, initialExampleModel } from '../../models/example.model';
import { HttpService } from '../../services/http.service';
import { SwalService } from '../../services/swal.service';
import { NgForm } from '@angular/forms';

@Component({

  selector: 'app-examples',
  imports: [
    SharedModule,
    ExamplePipe
  ],
  templateUrl: './examples.html',
  styleUrl: './examples.css'

})

export default class Examples {

  readonly examples = signal<ExampleModel[]>([{...initialExampleModel}])
  readonly createModel = signal<ExampleModel>({...initialExampleModel})
  readonly updateModel = signal<ExampleModel>({...initialExampleModel})
  search: string = ""

  // createModel: ExampleModel = new ExampleModel()
  // updateModel: ExampleModel = new ExampleModel()

  @ViewChild("createModalCloseBtn") createModalCloseBtn: ElementRef<HTMLButtonElement> | undefined
  @ViewChild("updateModalCloseBtn") updateModalCloseBtn: ElementRef<HTMLButtonElement> | undefined

  readonly #http = inject(HttpService)
  readonly #swal = inject(SwalService)

  getAll() {
    this.#http.post<ExampleModel[]>("Examples/GetAll", {}, (res) => {
      this.examples.set(res)
    });
  }

  create(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("Examples/Create", this.createModel(), (res) => {

        this.#swal.callToast(res)
        
        this.createModel.set({...initialExampleModel})
        this.createModalCloseBtn?.nativeElement.click()

        this.getAll()

      })

    }

  }

  deleteById(model: ExampleModel) {

    this.#swal.callSwal("Veriyi Sil?", `${model.field1} verisini silmek istiyor musunuz?`, () => {

      this.#http.post<string>("Examples/DeleteById", { id: model.id }, (res) => {

        this.getAll()
        this.#swal.callToast(res, "info");

      })

    })

  }

  get(model: ExampleModel) {
    this.updateModel.set({...model})
  }

  update(form: NgForm) {

    if (form.valid) {

      this.#http.post<string>("Examples/Update", this.updateModel(), (res) => {

        this.#swal.callToast(res, "info")
        this.updateModalCloseBtn?.nativeElement.click()
        this.getAll()

      })

    }

  }

}
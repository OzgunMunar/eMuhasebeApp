import { initialTypeModel, TypeModel } from "../commonmodels/smartenum-type.model"
import { CustomerModel, initialCustomerModel } from "../customermodels/customer.model"
import { InvoiceDetailModel } from "./invoicedetailmodels/invoicedetail.model"

export interface InvoiceModel {

    id: string,
    date: string,
    amount: number,
    invoiceNumber: string,
    type: TypeModel,
    typeValue: number,
    customerId: string,
    customer: CustomerModel,
    details: InvoiceDetailModel[],

    
    // ayrı değişken yaratmamak için burada tanımlıyorum.
    // Invoice sayfasında detay olarak girebilmek için.
    productId: string,
    quantity: number,
    price: number

}

export const initialInvoiceModel: InvoiceModel = {
    id: "",
    date: "",
    amount: 0,
    invoiceNumber: "",
    type: { ...initialTypeModel },
    typeValue: 0,
    customerId: "",
    customer: { ...initialCustomerModel },
    details: [],

    
    productId: "",
    quantity: 0,
    price: 0
}
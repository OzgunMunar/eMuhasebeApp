import { initialProductModel, ProductModel } from "../../productmodels/product.model"

export interface InvoiceDetailModel {

    id: string,
    invoiceId: string,
    productId: string,
    product: ProductModel,
    quantity: number,
    price: number

}

export const initialInvoiceDetailModel: InvoiceDetailModel = {
    id: "",
    invoiceId: "",
    productId: "",
    product: { ...initialProductModel },
    quantity: 0,
    price: 0
}
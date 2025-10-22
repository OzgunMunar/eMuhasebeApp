import { ProductDetailsModel } from "./productdetailsmodel/productdetails.model"

export interface ProductModel {
    
    id: string,
    name: string,
    deposit: number,
    withdrawal: number,
    details: ProductDetailsModel[]

}

export const initialProductModel: ProductModel = {
    id: "",
    name: "",
    deposit: 0,
    withdrawal: 0,
    details: []
}
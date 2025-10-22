export interface ProductDetailsModel {

    id: string,
    productId: string,
    date: string,
    description: string,
    deposit: number,
    withdrawal: number

}

export const initialProductDetailsModel: ProductDetailsModel = {

    id: "",
    productId: "",
    date: "",
    description: "",
    deposit: 0,
    withdrawal: 0
    
}
import { initialTypeModel, TypeModel } from "../commonmodels/smartenum-type.model"
import { CustomerDetailsModel, initialCustomerDetailsModel } from "./customerdetailsmodel/customerdetails.model"

export interface CustomerModel {

    id: string,
    name: string,
    type: TypeModel,
    typeValue: number,
    fullAddress: string,
    taxDepartment: string,
    taxNumber: string,
    depositAmount: number,
    withdrawalAmount: number
    details: CustomerDetailsModel[]

}

export const initialCustomerModel : CustomerModel = {

    id: "",
    name: "",
    type: {...initialTypeModel},
    typeValue: 1,
    fullAddress: "",
    taxDepartment: "",
    taxNumber: "",
    depositAmount: 9,
    withdrawalAmount: 0,
    details: []

}
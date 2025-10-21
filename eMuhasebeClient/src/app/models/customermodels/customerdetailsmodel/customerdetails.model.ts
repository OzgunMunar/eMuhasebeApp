import { initialTypeModel, TypeModel } from "../../commonmodels/smartenum-type.model"

export interface CustomerDetailsModel {

    id: string,
    type: TypeModel,
    typeValue: number,
    date: string,
    depositAmount: number,
    withdrawalAmount: number,
    description: string

}

export const initialCustomerDetailsModel : CustomerDetailsModel = {
    id: "",
    type: {...initialTypeModel},
    typeValue: 1,
    date: "",
    depositAmount: 0,
    withdrawalAmount: 0,
    description: ""
}
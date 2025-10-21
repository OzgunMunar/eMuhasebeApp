import { TypeModel, initialTypeModel } from "../commonmodels/smartenum-type.model"
import { CashRegisterDetailsModel } from "./cashregisterdetailsmodel/cashregisterdetails.model"

export interface CashRegisterModel {

    id: string,
    cashRegisterName: string,
    cashDepositAmount: number,
    cashWithdrawalAmount: number,
    currencyType: TypeModel,
    currencyTypeValue: number,
    cashRegisterDetails: CashRegisterDetailsModel[]
}

export const initialCashRegisterModel : CashRegisterModel = {
    id: "",
    cashRegisterName: "",
    cashDepositAmount: 0,
    cashWithdrawalAmount: 0,
    currencyType: { ...initialTypeModel },
    currencyTypeValue: 1,
    cashRegisterDetails: []
}
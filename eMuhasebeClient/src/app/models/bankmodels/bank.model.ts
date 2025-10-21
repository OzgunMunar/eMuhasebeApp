import { TypeModel, initialTypeModel } from "../commonmodels/smartenum-type.model"
import { BankDetailsModel } from "./bankdetailmodel/bankdetail.model"

export interface BankModel {
    id: string,
    bankName: string,
    iban: string,
    currencyType: TypeModel,
    currencyTypeValue: number,
    bankDepositAmount: number,
    bankWithdrawalAmount: number,
    bankDetails: BankDetailsModel[]
}

export const initialBankModel: BankModel = {
    id: "",
    bankName: "",
    iban: "",
    bankDepositAmount: 0,
    bankWithdrawalAmount: 0,
    currencyType: { ...initialTypeModel },
    currencyTypeValue: 1,
    bankDetails: []
}
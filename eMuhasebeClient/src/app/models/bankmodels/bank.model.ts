import { CurrencyTypeModel, initialCurrencyTypeModel } from "../commonmodels/currency-type.model"
import { BankDetailsModel } from "./bankdetailmodel/bankdetail.model"

export interface BankModel {
    id: string,
    bankName: string,
    iban: string,
    currencyType: CurrencyTypeModel,
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
    currencyType: { ...initialCurrencyTypeModel },
    currencyTypeValue: 1,
    bankDetails: []
}
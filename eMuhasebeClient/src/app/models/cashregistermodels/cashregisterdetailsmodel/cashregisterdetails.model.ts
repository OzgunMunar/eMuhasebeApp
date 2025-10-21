import { BankModel, initialBankModel } from "../../bankmodels/bank.model"
import { CashRegisterModel, initialCashRegisterModel } from "../cashregister.model"

export interface CashRegisterDetailsModel {

    id: string,
    cashRegisterId: string,
    openedDate: string,
    type: number,
    recordType: number,
    amount: number,
    cashDepositAmount: number,
    oppositeAmount: number,
    cashWithdrawalAmount: number,
    cashRegisterDetailId: string,
    bankDetailId: string,
    oppositeCashRegisterId?: string | null,
    oppositeBankId?: string | null,
    oppositeCashRegister: CashRegisterModel,
    oppositeBank: BankModel,
    description: string,

}

export const initialCashRegisterDetailsModel: CashRegisterDetailsModel = {
    id: "",
    cashRegisterId: "",
    openedDate: "",
    type: 0,
    recordType: 0,
    amount: 0,
    oppositeAmount: 0,
    cashDepositAmount: 0,
    cashWithdrawalAmount: 0,
    cashRegisterDetailId: "",
    bankDetailId: "",
    oppositeCashRegisterId: "",
    oppositeBankId: "",
    description: "",
    oppositeCashRegister: { ...initialCashRegisterModel },
    oppositeBank: {...initialBankModel}

}
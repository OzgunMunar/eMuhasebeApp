import { CashRegisterModel, initialCashRegisterModel } from "../../cashregistermodels/cashregister.model";
import { BankModel, initialBankModel } from "../bank.model";

export interface BankDetailsModel {

    id: string,
    bankId: string,
    openedDate: string,
    type: number,
    amount: number,
    bankDepositAmount: number,
    bankWithdrawalAmount: number,
    bankDetailId: string,
    oppositeBankId?: string | null,
    oppositeCashRegisterId?: string | null,
    cashRegisterDetailId: string,
    oppositeBank: BankModel,
    oppositeCash: CashRegisterModel,
    description: string,
    oppositeAmount: number,
    recordType: number,
    
}

export const initialBankDetailsModel: BankDetailsModel = {
    id: "",
    bankId: "",
    openedDate: "",
    type: 0,
    amount: 0,
    bankDepositAmount: 0,
    bankWithdrawalAmount: 0,
    bankDetailId: "",
    oppositeBankId: "",
    oppositeCashRegisterId: "",
    cashRegisterDetailId: "",
    oppositeBank: {...initialBankModel},
    oppositeCash: {...initialCashRegisterModel},
    description: "",
    oppositeAmount: 0,
    recordType: 0
}
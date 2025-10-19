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
    oppositeCashRegisterId?: string | null,
    oppositeCashRegister: CashRegisterModel,
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
    oppositeCashRegisterId: "",
    description: "",
    oppositeCashRegister: { ...initialCashRegisterModel },

}
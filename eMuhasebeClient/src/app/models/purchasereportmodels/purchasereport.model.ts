export interface PurchaseReportModel {
    dates: string[],
    amounts: number[]
}


export const initialPurchaseReportModel: PurchaseReportModel = {
    dates: [],
    amounts: []
}
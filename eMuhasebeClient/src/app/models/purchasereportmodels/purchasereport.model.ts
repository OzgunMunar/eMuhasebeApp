export interface PurchaseReportModel {
    months: string[],
    amounts: number[]
}


export const initialPurchaseReportModel: PurchaseReportModel = {
    months: [],
    amounts: []
}
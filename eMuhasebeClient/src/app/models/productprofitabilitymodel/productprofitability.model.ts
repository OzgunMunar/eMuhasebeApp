export interface ProductProfitabilityModel {
    
    id: string,
    name: string,
    depositPrice: number,
    withdrawalPrice: number,
    profitPercent: number,
    profit: number

}

export const initialProductProfitabilityModel: ProductProfitabilityModel = {
    id: "",
    name: "",
    depositPrice: 0,
    withdrawalPrice: 0,
    profitPercent: 0,
    profit: 0
}
export interface CurrencyTypeModel {
    value: number,
    name: string
}

export const initialCurrencyTypeModel : CurrencyTypeModel = {
    value: 1,
    name: ""
}

export const CurrencyTypes: CurrencyTypeModel[] = 
[
    {
        value: 1,
        name: "TL"
    },
    {
        value: 2,
        name: "USD"
    },
    {
        value: 3,
        name: "EUR"
    }
]
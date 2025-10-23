export interface TypeModel {
    value: number,
    name: string
}

export const initialTypeModel : TypeModel = {
    value: 1,
    name: ""
}

export const CurrencyTypes: TypeModel[] = 
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

export const CustomerTypes: TypeModel[] = 
[
    {
        value: 1,
        name: "Ticari Alıcılar"
    },
    {
        value: 2,
        name: "Ticari Satıcılar"
    },
    {
        value: 3,
        name: "Personel"
    },
    {
        value: 4,
        name: "Şirket Ortakları"
    }
]

export const CustomerDetailTypes: TypeModel[] = [

    {
        value: 1,
        name: "Banka"
    },
    {
        value: 2,
        name: "Kasa"
    },
    {
        value: 3,
        name: "Alış Faturası"
    },
    {
        value: 4,
        name: "Satış Faturası"
    }

]

export const InvoiceTypes: TypeModel[] = 
[
    {
        value: 1,
        name: "Alış Faturası"
    },
    {
        value: 2,
        name: "Satış Faturası"
    }
]
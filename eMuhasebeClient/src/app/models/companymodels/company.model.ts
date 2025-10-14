import { DatabaseModel, initialDatabaseModel } from "./database.model"

export interface CompanyModel {

    id: string,
    companyName: string,
    fullAddress: string,
    taxDepartment: string,
    taxNumber: string,
    database: DatabaseModel

}

export const initialCompanyModel : CompanyModel = {
    id: "",
    companyName: "",
    fullAddress: "",
    taxDepartment: "",
    taxNumber: "",
    database: { ...initialDatabaseModel }
}
import { CompanyModel, initialCompanyModel } from "../companymodels/company.model"

export interface CompanyUserModel{

   appUserId: string,
   companyId: string,
   company: CompanyModel

}

export const initialCompanyUserModel : CompanyUserModel = {
    appUserId: "",
    companyId: "",
    company: {...initialCompanyModel}
}
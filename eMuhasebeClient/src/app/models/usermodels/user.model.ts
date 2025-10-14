import { CompanyModel } from "../companymodels/company.model"
import { CompanyUserModel } from "../companyusermodel/companyuser.model"

export interface UserModel{

    id: string,
    name: string,
    firstName: string,
    lastName: string,
    fullName: string,
    password: string | null,
    userName: string,
    email: string,
    companies: CompanyModel[],
    companyId: string,
    companyIds: string[],
    companyUsers: CompanyUserModel[]

}

export const initialUserModel : UserModel = {
    id: "",
    name: "",
    firstName: "",
    lastName: "",
    fullName: "",
    password: "",
    userName: "",
    email: "",
    companies: [],
    companyId: "",
    companyIds: [],
    companyUsers: []
}
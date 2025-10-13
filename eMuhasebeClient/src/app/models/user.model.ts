export interface UserModel{

    id: string,
    name: string,
    firstName: string,
    lastName: string,
    fullName: string,
    password: string | null,
    userName: string,
    email: string

}

export const initialUserModel : UserModel = {

    id: "",
    name: "",
    firstName: "",
    lastName: "",
    fullName: "",
    password: "",
    userName: "",
    email: ""
    
}
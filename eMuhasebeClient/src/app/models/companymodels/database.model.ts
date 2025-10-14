export interface DatabaseModel {

    server: string,
    databaseName: string,
    userId: string,
    password: string 

}

export const initialDatabaseModel : DatabaseModel = {
    server: "",
    databaseName: "",
    userId: "",
    password: ""
}
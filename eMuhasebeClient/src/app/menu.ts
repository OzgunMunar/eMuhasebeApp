export class MenuModel{
    name: string = "";
    icon: string = "";
    url: string = "";
    isTitle: boolean = false;
    subMenus: MenuModel[] = [];
    isAdminOnly: boolean = false;
}

export const Menus: MenuModel[] = [
    {
        name: "Ana Sayfa",
        icon: "fa-solid fa-home",
        url: "/",
        isTitle: false,
        subMenus: [],
        isAdminOnly: false
    },
    {
        name: "Admin",
        icon: "",
        url: "",
        isTitle: true,
        subMenus: [],
        isAdminOnly: true
    },
    {
        name: "Kullanıcılar",
        icon: "fa-solid fa-users",
        url: "/users",
        isTitle: false,
        subMenus: [],
        isAdminOnly: true
    },
    {
        name: "Şirketler",
        icon: "fa-solid fa-city",
        url: "/companies",
        isTitle: false,
        subMenus: [],
        isAdminOnly: true
    }
]
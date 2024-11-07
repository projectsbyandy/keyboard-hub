import {Axios} from "axios";
import {UserDto} from "../models/userDto.ts";
import authApiConnector from "../api/authApiConnector.ts";

export async function generateToken() {
    const user : UserDto = {
        username: "andy",
        email: "andy.peters@test.com",
        password: "tester123"
    }

    const token = await authApiConnector.getToken(user)
    sessionStorage.setItem("token", token);
}

export function setAuthHeader(client : Axios) {
  client.defaults.headers.common["Authorization"] = `Bearer ${sessionStorage.getItem("token")}`;
}

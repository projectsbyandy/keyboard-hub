import axios, {AxiosResponse} from "axios"
import {VENDOR_API_BASE_URL} from "../../config";
import { UserDto } from "../models/userDto";

const authApiConnector = {
    
    getToken: async(user : UserDto) : Promise<string> => {
        try {
            const response : AxiosResponse<string> = await axios.post(`${VENDOR_API_BASE_URL}/api/authenticate/login`, user)
            return response.data   
        } catch(error) {
            console.log('Error authenticating:', error);
            throw error;
        }
    }
}

export default authApiConnector
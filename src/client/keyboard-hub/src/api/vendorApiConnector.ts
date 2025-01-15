import { VendorDto } from "../models/vendorDto";
import axios, {AxiosResponse} from "axios";
import {VENDOR_API_BASE_URL} from "../../config";
import {generateToken, setAuthHeader} from "../services/authService.ts";

const vendorApiConnector = {
    getVendors: async () : Promise<VendorDto[]> => {
        try {
            
            await generateToken();
            setAuthHeader(axios);

            const response : AxiosResponse<VendorDto[]>
                = await axios.get(`${VENDOR_API_BASE_URL}/api/vendor/all`);


            return response.data;
        } catch (error) {
            console.log('Error fetching vendors:', error);
            throw error;
        }
    },

    getVendor: async (vendorId: string) : Promise<VendorDto> => {
        try {
            const response : AxiosResponse<VendorDto>
                = await axios.get(`${VENDOR_API_BASE_URL}/api/vendor/${vendorId}`);

            return response.data;
        } catch (error) {
            console.log('Error fetching vendors:', error);
            throw error;
        }
    },
    
    createVendor: async (vendor: VendorDto) : Promise<void> => {
        try {
            await axios.post(`${VENDOR_API_BASE_URL}/api/vendor`, vendor)
        } catch(error) {
            console.log('Error creating vendor:', error);
            throw error;        
        }
    },
    
    editVendor: async (vendor: VendorDto) : Promise<void> => {
        try {
            await axios.put(`${VENDOR_API_BASE_URL}/api/vendor`, vendor)
        } catch(error) {
            console.log('Error editing vendor:', error);
            throw error;
        }
    },

    deleteVendor: async (vendorId: string) : Promise<void> => {
        try {
            await axios.delete(`${VENDOR_API_BASE_URL}/api/vendor/${vendorId}`)
        } catch(error) {
            console.log('Error deleting vendor:', error);
            throw error;
        }
    }
}

 export default vendorApiConnector;
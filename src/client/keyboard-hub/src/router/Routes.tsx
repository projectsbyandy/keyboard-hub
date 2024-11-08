import {createBrowserRouter, RouteObject} from "react-router-dom";
import App from "../App.tsx";
import VendorForm from "../components/vendors/VendorForm.tsx";

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App/>,
        children: [
            { path: 'createVendor', element: <VendorForm key={'create'}/>},
            { path: 'editVendor/:id', element: <VendorForm key={'edit'}/>},
        ]
    }
]

export const router = createBrowserRouter(routes)
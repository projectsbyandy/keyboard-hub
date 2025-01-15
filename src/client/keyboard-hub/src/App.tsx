import './App.css'
import VendorTable from './components/vendors/VendorTable'
import {Outlet, useLocation} from "react-router-dom";
import {Container} from "semantic-ui-react";
import {useEffect} from "react";
import {setupErrorHandlingInterceptor} from "./interceptors/axiosinterceptor.ts";

function App() {
    const location = useLocation();

    useEffect(()=> {
        setupErrorHandlingInterceptor();
    })

  return (
    <>
        { location.pathname === '/' ? <VendorTable /> : (
            <Container className="container-style">
                <Outlet/>
            </Container>
        )}
    </>
  )
}

export default App

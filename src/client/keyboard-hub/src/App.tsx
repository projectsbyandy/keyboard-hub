import './App.css'
import VendorTable from './components/vendors/VendorTable'
import {Outlet, useLocation} from "react-router-dom";
import {Container} from "semantic-ui-react";

function App() {
    const location = useLocation();

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

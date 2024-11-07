import {useEffect, useState} from "react";
import { VendorDto } from "../../models/vendorDto";
import vendorApiConnector from "../../api/vendorApiConnector";
import {Button, Container } from "semantic-ui-react";
import VendorTableItem from "./VendorTableItem";

export default function VendorTable() {
    
    const [vendors, setVendors] = useState<VendorDto[]>([]);
    
    useEffect(()=> {
        const fetchData = async() => {
            const fetchedVendors = await vendorApiConnector.getVendors();
            setVendors(fetchedVendors);
        }
        
        fetchData();
    }, []);
    
    return (
        <>
            <Container className="container-style">
                <table className="ui inverted table">
                    <thead style={{textAlign: 'center'}}>
                    <tr>
                        <th>Id</th>
                        <th>Name</th>
                        <th>YearsActive</th>
                        <th>Live</th>
                        <th>Action</th>
                    </tr>
                    </thead>
                    <tbody>
                    {vendors.length !== 0 && (
                        vendors.map((vendor, index) => (
                            <VendorTableItem key={index} vendor={vendor} />
                        ))
                    )}
                    </tbody>
                </table>
                <Button floated="right" type="button" content="Create Vendor" postive="true"></Button>
            </Container>
        </>
    )
}
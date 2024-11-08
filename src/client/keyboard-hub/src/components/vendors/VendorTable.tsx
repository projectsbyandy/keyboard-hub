import {useEffect, useState} from "react";
import {Button, Container } from "semantic-ui-react";
import { VendorDto } from "../../models/vendorDto";
import VendorTableItem from "./VendorTableItem";
import vendorApiConnector from "../../api/vendorApiConnector";

export default function VendorTable() {

    const [vendors, setVendors] = useState<VendorDto[]>([]);

    const fetchData = async() => {
        const fetchedVendors = await vendorApiConnector.getVendors();
        setVendors(fetchedVendors);
    }

    useEffect(()=> {
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
                    <tbody style={{textAlign: 'center'}}>
                    {vendors.length !== 0 && (
                        vendors.map((vendor, index) => (
                            <VendorTableItem key={index} vendor={vendor} />
                        ))
                    )}
                    </tbody>
                </table>
                <Button floated="right" type="button" color="green" content="Create Vendor" postive="true"></Button>
            </Container>
        </>
    )
}
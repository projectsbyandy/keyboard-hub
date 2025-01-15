import {NavLink, useNavigate, useParams} from "react-router-dom";
import {ChangeEvent, useEffect, useState} from "react";
import {VendorDto} from "../../models/vendorDto.ts";
import vendorApiConnector from "../../api/vendorApiConnector.ts";
import {Button, Form, Segment} from "semantic-ui-react";

interface VendorFormProps {
    action?: string
}

export default function VendorForm({action}: VendorFormProps) {

    const {id} = useParams();
    const navigate = useNavigate();

    const [vendor, setVendor] = useState<VendorDto>({
        id: crypto.randomUUID(),
        name: '',
        description: '',
        yearsActive: 0,
        isLive: false,
    });

    useEffect(() => {
        if (id) {
            vendorApiConnector.getVendor(id)
                .then(vendor => setVendor(vendor))
        }
    }, [id]);

    function handleSubmit() {
        if (action === "create") {
            vendorApiConnector.createVendor(vendor)
                .then(() => navigate(('/')));
        } else {
            vendorApiConnector.editVendor(vendor)
                .then(() => navigate(('/')));
        }
    }


    function handleInputChange(event: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
        const {name, value} = event.target;
        setVendor({...vendor, [name]: value})
    }

    return (
        <>
            <Segment clearing inverted>
                <Form onSubmit={handleSubmit} autoComplete="off" className='ui inverted form'>
                    <Form.Input placeholder='Id' name='id' value={vendor.id} onChange={handleInputChange}/>
                    <Form.Input placeholder='Name' name='name' value={vendor.name} onChange={handleInputChange}/>
                    <Form.TextArea placeholder='Description' name='description' value={vendor.description}
                                   onChange={handleInputChange}/>
                    <Form.Input placeholder='Years Active' name='yearsActive' value={vendor.yearsActive}
                                onChange={handleInputChange}/>
                    <Form.Input placeholder='Is Live' name='isLive' value={vendor.isLive.toString()}
                                onChange={handleInputChange}/>
                    <Button floated='right' positive type="submit" content='Submit'/>
                    <Button as={NavLink} to='/' floated='right' type="button" content='Cancel'/>
                </Form>
            </Segment>
        </>
    )
}
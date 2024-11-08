import vendorApiConnector from "../../api/vendorApiConnector";
import { VendorDto } from "../../models/vendorDto"
import {Button} from "semantic-ui-react";
import {NavLink} from "react-router-dom";

interface Props {
    vendor : VendorDto
}

export default function VendorTableItem({vendor}: Props) {
    return (
        <>
            <tr className="centre aligned">
                <td data-label="Id">{vendor.id}</td>
                <td data-label="Name">{vendor.name}</td>
                <td data-label="Description">{vendor.description}</td>
                <td data-label="Years Active">{vendor.yearsActive}</td>
                <td data-label="Live">{vendor.isLive.toString()}</td>
                <td data-label="Action">
                    <Button as={NavLink} to={`editVendor/${vendor.id}`} color="yellow" type="submit">
                        Edit
                    </Button>
                    <Button type="button" negative onClick={async () => {
                        await vendorApiConnector.deleteVendor(vendor.id!)
                        window.location.reload();
                    }}>
                        Delete
                    </Button>
                </td>
            </tr>
        </>
    )
}
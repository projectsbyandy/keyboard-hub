import vendorApiConnector from "../../api/vendorApiConnector";
import { VendorDto } from "../../models/vendorDto"
import {Button} from "semantic-ui-react";

interface Props {
    vendor : VendorDto
}

export default function VendorTableItem({vendor}: Props) {
    return (
        <>
            <tr className="centre aligned">
                <td data-label="Id">{vendor.id}</td>
                <td data-label="Name">{vendor.name}</td>
                <td data-label="Years Active">{vendor.yearsActive}</td>
                <td data-label="Live">{vendor.live.toString()}</td>
                <td data-label="Action">
                    <Button color="yellow" type="submit">
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
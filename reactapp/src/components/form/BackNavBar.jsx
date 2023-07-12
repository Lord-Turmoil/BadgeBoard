import { useNavigate } from "react-router-dom";

import { Fab } from "@mui/material";
import ArrowBackIosRoundedIcon from '@mui/icons-material/ArrowBackIosRounded';

export default function BackNavBar() {
    var navigate = useNavigate();

    return (
        <div style={{width: '90%', margin: '10px auto'}}>
            <Fab size='medium' color='primary' onClick={() => { navigate(-1); }}>
                <ArrowBackIosRoundedIcon />
            </Fab>
        </div>
    );
}
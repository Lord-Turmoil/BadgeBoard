import { Fab } from "@mui/material";
import ArrowBackIosRoundedIcon from '@mui/icons-material/ArrowBackIosRounded';

export default function BackNavBar() {
    return (
        <div style={{width: '90%', margin: '10px auto'}}>
            <Fab size='medium' color='primary'>
                <ArrowBackIosRoundedIcon />
            </Fab>
        </div>
    );
}
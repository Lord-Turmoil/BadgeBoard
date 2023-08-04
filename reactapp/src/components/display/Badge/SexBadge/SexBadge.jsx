import MaleRoundedIcon from '@mui/icons-material/MaleRounded';
import FemaleRoundedIcon from '@mui/icons-material/FemaleRounded';
import QuestionMarkRoundedIcon from '@mui/icons-material/QuestionMarkRounded';

import CornerBadge from "~/components/display/Badge/CornerBadge/CornerBadge"

export default function SexBadge({ sex }) {
    const renderSex = (s) => {
        switch (s) {
            case 1: return (<MaleRoundedIcon color="primary" />);
            case 2: return (<FemaleRoundedIcon color="secondary" />);
            default: return (<QuestionMarkRoundedIcon color="error" />);
        }
    }

    return (
        <CornerBadge className='SexBadge' right bottom size='25px' sx={{ backdropFilter: 'blur(1px)' }}>
            {renderSex(sex)}
        </CornerBadge>
    );
};
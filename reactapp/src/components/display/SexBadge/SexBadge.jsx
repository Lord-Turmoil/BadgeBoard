import MaleRoundedIcon from '@mui/icons-material/MaleRounded';
import FemaleRoundedIcon from '@mui/icons-material/FemaleRounded';
import QuestionMarkRoundedIcon from '@mui/icons-material/QuestionMarkRounded';

import CornerBadge from "../CornerBadge/CornerBadge";

export default function SexBadge({ sex }) {
    const renderSex = (s) => {
        switch (s) {
            case 1: return (<MaleRoundedIcon color="primary" />);
            case 2: return (<FemaleRoundedIcon color="secondary" />);
            default: return (<QuestionMarkRoundedIcon color="error" />);
        }
    }

    return (
        <CornerBadge right bottom>
            {renderSex(sex)}
        </CornerBadge>
    );
};
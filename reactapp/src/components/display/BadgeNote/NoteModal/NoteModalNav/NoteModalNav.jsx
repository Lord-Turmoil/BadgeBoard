import { Checkbox, IconButton } from '@mui/material';
import './NoteModalNav.css';
import NoteModalTitle from './NoteModalTitle/NoteModalTitle';
import DeleteForeverRoundedIcon from '@mui/icons-material/DeleteForeverRounded';
import DriveFileMoveRoundedIcon from '@mui/icons-material/DriveFileMoveRounded';
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';

export default function NoteModalNav({
    badge = null,
    onBadgeChange = null,
    isOwner = false
}) {
    return (
        <div className="NoteModalNav">
            <NoteModalTitle badge={badge} />
            <div className="NoteModalNav__actionWrapper">
                <div className="NoteModalNav__action NoteModalNav__button">
                    <Checkbox
                        size="large"
                        color='error'
                        icon={<VisibilityIcon />}
                        checkedIcon={<VisibilityOffIcon />}
                    />
                </div>
                <div className="NoteModalNav__action NoteModalNav__button">
                    <IconButton aria-label="delete" size="large">
                        <DriveFileMoveRoundedIcon fontSize="inherit" />
                    </IconButton>
                </div>
                <div className="NoteModalNav__action NoteModalNav__button">
                    <IconButton aria-label="delete" size="large">
                        <DeleteForeverRoundedIcon fontSize="inherit" />
                    </IconButton>
                </div>
            </div>
        </div >
    );
};
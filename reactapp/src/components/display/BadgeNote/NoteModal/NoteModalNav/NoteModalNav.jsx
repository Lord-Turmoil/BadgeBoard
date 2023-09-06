import { Checkbox, IconButton } from '@mui/material';
import './NoteModalNav.css';
import NoteModalTitle from './NoteModalTitle/NoteModalTitle';
import DeleteForeverRoundedIcon from '@mui/icons-material/DeleteForeverRounded';
import DriveFileMoveRoundedIcon from '@mui/icons-material/DriveFileMoveRounded';
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import { useState } from 'react';
import api from '~/services/api';
import notifier from '~/services/notifier';

export default function NoteModalNav({
    badge = null,
    onBadgeChange = null,
    isOwner = false
}) {
    const isVisible = badge ? badge.isPublic : false;

    const [visibilityChecked, setVisibilityChecked] = useState(isVisible);
    const onChangeVisibility = async (event) => {
        const checked = !visibilityChecked;
        const newBadge = {
            id: badge.id,
            badge: {
                ...badge,
                isPublic: checked
            }
        }
        const status = await changeBadgeVisibility(newBadge.badge);
        if (!status) {
            return;
        }

        setVisibilityChecked(checked);
        onBadgeChange && onBadgeChange({
            type: "update",
            value: newBadge
        });
    }

    async function changeBadgeVisibility(b) {
        const dto = await api.post("badge/update", {
            id: b.id,
            style: null,
            isPublic: b.isPublic
        })
        console.log("🚀 > changeBadgeVisibility > dto:", dto);
        notifier.auto(dto.meta, "Visibility changed", "Failed to change visibility");
        return dto.meta.status == 0;
    }

    return (
        <div className="NoteModalNav">
            <NoteModalTitle badge={badge} />
            {isOwner &&
                <div className="NoteModalNav__actionWrapper">
                    <div className="NoteModalNav__action NoteModalNav__button">
                        <Checkbox
                            sx={{ '& .MuiSvgIcon-root': { fontSize: 34 } }}
                            color='error'
                            icon={<VisibilityIcon />}
                            checkedIcon={<VisibilityOffIcon />}
                            checked={!visibilityChecked}
                            onChange={onChangeVisibility}
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
            }
        </div >
    );
};
import { Button, Checkbox, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, IconButton } from '@mui/material';
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
    isOwner = false,
    onClose = null,
}) {
    // edit: isPublic (visibility)
    const isVisible = badge ? badge.isPublic : false;
    const [visibilityChecked, setVisibilityChecked] = useState(isVisible);
    const onChangeVisibility = async (event) => {
        const checked = !visibilityChecked;
        const badgeAction = {
            type: "update",
            value: {
                id: badge.id,
                badge: {
                    ...badge,
                    isPublic: checked
                }
            }
        };

        const status = await changeBadgeVisibility(badgeAction.value.badge);
        if (!status) {
            return;
        }

        setVisibilityChecked(checked);
        onBadgeChange && onBadgeChange(badgeAction);
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

    // edit: deletion
    const [deleteConfirmOpen, setDeleteConfirmOpen] = useState(false);

    const onDelete = async (event) => {
        setDeleteConfirmOpen(true);
    }

    const onCloseDeleteConfirm = () => {
        setDeleteConfirmOpen(false);
    }

    const onClickConfirmDelete = async () => {
        const badgeAction = {
            type: "delete",
            value: {
                id: badge.id
            }
        };

        const status = await deleteBadge(badgeAction.value.id);
        if (!status) {
            return;
        }

        onBadgeChange && onBadgeChange(badgeAction);
        onCloseDeleteConfirm();
        onClose && onClose();
    }

    const onClickCancelDelete = () => {
        onCloseDeleteConfirm();
    }

    async function deleteBadge(id) {
        const dto = await api.post("badge/delete", {
            badges: [id],
            force: false
        });
        notifier.auto(dto.meta, "Badge deleted", "Failed to delete badge");
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
                        <IconButton aria-label="delete" size="large" onClick={onDelete}>
                            <DeleteForeverRoundedIcon fontSize="inherit" />
                        </IconButton>
                    </div>
                </div>
            }
            <Dialog open={deleteConfirmOpen} onClose={onCloseDeleteConfirm}>
                <DialogTitle>Delete Badge?</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Deletion of badges CANNOT be undone, think twice. ⚠️
                    </DialogContentText>
                </DialogContent>
                <DialogActions>
                    <Button onClick={onClickConfirmDelete}>Proceed</Button>
                    <Button onClick={onClickCancelDelete} autoFocus>Cancel</Button>
                </DialogActions>
            </Dialog>
        </div >
    );
};
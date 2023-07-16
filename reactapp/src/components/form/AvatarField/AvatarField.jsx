import { Avatar } from "@mui/material";
import CameraAltRoundedIcon from '@mui/icons-material/CameraAltRounded';

import AvatarUrl from "~/services/user/avatarUrl";
import './AvatarField.css';
import { useEffect, useState } from "react";
import PopupModal from "~/components/layout/PopupModal";

export default function AvatarField({
    size = 100,
    src,
    onAvatarChange = null,
    disabled = true
}) {
    // propertied
    const [imageUrl, setImageUrl] = useState(src);
    const [imageData, setImageData] = useState(src);
    const [showDialog, setShowDialog] = useState(false);

    useEffect(() => {
        console.log("🚀 > useEffect > imageUrl:", imageUrl);
    }, [imageUrl]);

    useEffect(() => {
        console.log("🚀 > useEffect > imageData:", imageData);
    }, [imageData]);

    const onClickUpload = (event) => {
        const input = document.createElement('input')
        input.type = 'file'
        input.accept = 'image/*'
        input.addEventListener('change', handleFileSelect)
        input.click();
    }

    const handleFileSelect = async (event) => {
        event.preventDefault();
        try {
            let files;
            if (event.dataTransfer) {
                files = event.dataTransfer.files;
            } else if (event.target) {
                files = event.target.files;
            }
            const reader = new FileReader();
            if (!reader) {
                return;
            }
            reader.onload = () => {
                setImageData(reader.result?.toString());
                setShowDialog(true);
            };
            reader.readAsDataURL(files[0]);
        } catch (error) {
            console.log("🚀 > handleFileSelect > error:", error);
        }
    }

    return (
        <div className="AvatarField">
            <Avatar sx={{ width: size, height: size }} src={AvatarUrl.get(imageData)} alt="Avatar" />
            <div className={`AvatarField__mask${disabled ? "" : " AvatarField__active"}`}>
                <div className="AvatarField__upload" onClick={disabled ? null : onClickUpload}>
                    <CameraAltRoundedIcon />
                </div>
            </div>
            <PopupModal
                open={showDialog}
                title="Edit Avatar"
                onClose={() => { setShowDialog(false) }}
            >
                <div>Hello there!</div>
            </PopupModal>
        </div>
    );
};

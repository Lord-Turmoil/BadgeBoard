import { useCallback, useState } from "react";
import Cropper from "react-easy-crop";

import { Slider, Stack } from "@mui/material";
import AddRoundedIcon from '@mui/icons-material/AddRounded';
import RemoveRoundedIcon from '@mui/icons-material/RemoveRounded';

import './ImageCrop.css'

const ZOOM_RATIO = 100;

export default function ImageCrop({
    image = null,
    onCroppedAreaChange = null
}) {
    const [crop, setCrop] = useState({ x: 0, y: 0 })
    const [zoom, setZoom] = useState(1)
    const [sliderValue, setSliderValue] = useState(100);

    const onSliderValueChange = (event, value) => {
        setSliderValue(value);
        setZoom(value / ZOOM_RATIO);
    }

    const onCropperZoomChange = (value) => {
        setZoom(value);
        setSliderValue(value * ZOOM_RATIO);
    }

    const onCropComplete = useCallback((croppedArea, croppedAreaPixels) => {
        // area is floating number, pixels are integral
        onCroppedAreaChange && onCroppedAreaChange(croppedAreaPixels);
    }, []);

    return (
        <div className="ImageCrop">
            <div className="ImageCrop__cropper">
                {image &&
                    <Cropper
                        image={image}
                        crop={crop}
                        zoom={zoom}
                        maxZoom={5}
                        minZoom={1}
                        aspect={1}
                        onZoomChange={onCropperZoomChange}
                        onCropChange={setCrop}
                        onCropComplete={onCropComplete}
                        cropShape="rect"
                        objectFit="cover"
                    />
                }
            </div>
            <Stack spacing={2} direction="row" alignItems="center">
                <RemoveRoundedIcon />
                <Slider color="secondary" min={100} max={500} value={sliderValue} onChange={onSliderValueChange} />
                <AddRoundedIcon />
            </Stack>
        </div>
    );
};
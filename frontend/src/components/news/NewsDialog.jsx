import { useState, useEffect } from "react";

import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Stack,
  TextField,
  Box,
} from "@mui/material";

export default function NewsDialog({
  open,
  onClose,
  onSave,
  mode = "create",
  news = null,
}) {
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");
  const [expirationDate, setExpirationDate] = useState("");
  const [coverImage, setCoverImage] = useState(null);

  useEffect(() => {
    if (news) {
      setTitle(news.title || "");
      setContent(news.content || "");
      setExpirationDate(
        news.expirationDate
          ? news.expirationDate.split("T")[0]
          : ""
      );
    } else {
      setTitle("");
      setContent("");
      setExpirationDate("");
      setCoverImage(null);
    }
  }, [news, open]);

  const handleSave = () => {
    const formData = new FormData();

    formData.append("Title", title);
    formData.append("Content", content);

    if (expirationDate)
      formData.append("ExpirationDate", expirationDate);

    if (coverImage)
      formData.append("CoverImage", coverImage);

    onSave(formData);

    setTitle("");
    setContent("");
    setExpirationDate("");
    setCoverImage(null);
  };

  return (
    <Dialog
      open={open}
      onClose={onClose}
      maxWidth="md"
      fullWidth
    >
      <DialogTitle>
        {mode === "create" && "إضافة خبر"}
        {mode === "view" && "عرض الخبر"}
        {mode === "edit" && "تعديل الخبر"}
      </DialogTitle>

      <DialogContent>

        <Stack spacing={3} mt={1}>

          <TextField
            label="عنوان الخبر"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            fullWidth
            disabled={mode === "view"}
          />

          <TextField
            label="المحتوى"
            multiline
            rows={6}
            value={content}
            onChange={(e) => setContent(e.target.value)}
            fullWidth
            disabled={mode === "view"}
          />

          <TextField
            label="تاريخ الانتهاء (اختياري)"
            type="date"
            fullWidth
            InputLabelProps={{
              shrink: true,
            }}
            inputProps={{
                style: {
                direction: "ltr",
                textAlign: "left",
                }, }}
            value={expirationDate}
            onChange={(e) =>
              setExpirationDate(e.target.value)
            }
            disabled={mode === "view"}
          />

          {mode !== "view" && (
        <Button
          variant="outlined"
          component="label"
        >
          اختيار صورة الغلاف

          <input
            hidden
            type="file"
            accept="image/*"
            onChange={(e) =>
              setCoverImage(e.target.files[0])
            }
          />
        </Button>
      )}

      {mode === "view" && (
      <>
        {news?.coverImagePath ? (
          <Box
            component="img"
            src={`http://localhost:5202/${news.coverImagePath}`}
            alt="Cover"
            sx={{
              width: "100%",
              maxHeight: 250,
              objectFit: "cover",
              borderRadius: 2,
              border: "1px solid #ddd",
            }}
          />
        ) : (
          <Box
            sx={{
              p: 2,
              textAlign: "center",
              border: "1px dashed #ccc",
              borderRadius: 2,
              color: "text.secondary",
            }}
          >
            لا توجد صورة غلاف
          </Box>
        )}
      </>
    )}

        </Stack>

      </DialogContent>

      <DialogActions>

    <Button onClick={onClose}>
      {mode === "view" ? "إغلاق" : "إلغاء"}
    </Button>

    {mode !== "view" && (
      <Button
        variant="contained"
        onClick={handleSave}
      >
        حفظ
      </Button>
    )}

  </DialogActions>

    </Dialog>
  );
}
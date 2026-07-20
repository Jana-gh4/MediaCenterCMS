import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    TextField,
    Table,
    TableHead,
    TableRow,
    TableCell,
    TableBody,
    Button,
    Stack,
} from "@mui/material";

import {
    approveRequest,
    rejectRequest,
} from "../../services/approval";

import { useState } from "react";

export default function ApprovalsTable({
    approvals,
    refresh,
}) {
    const handleApprove = async (id) => {
        try {
            await approveRequest(id);
            refresh();
        } catch {
            alert("Failed to approve request.");
        }
    };

    const handleReject = (id) => {
        setSelectedId(id);
        setReason("");
        setRejectDialogOpen(true);
    };

    const confirmReject = async () => {
        if (!reason.trim()) {
            alert("يرجى إدخال سبب الرفض.");
            return;
        }

        try {
            await rejectRequest(selectedId, reason);

            setRejectDialogOpen(false);
            refresh();

        } catch {
            alert("فشل في رفض الطلب.");
        }
    };
    const [rejectDialogOpen, setRejectDialogOpen] = useState(false);
    const [selectedId, setSelectedId] = useState(null);
    const [reason, setReason] = useState("");

    return (
        <>
        <Table>
            <TableHead>
                <TableRow>
                    <TableCell>النوع</TableCell>
                    <TableCell>العنوان</TableCell>
                    <TableCell>مقدم الطلب</TableCell>
                    <TableCell>تاريخ الطلب</TableCell>
                    <TableCell>الحالة</TableCell>
                    <TableCell align="center">الإجراءات</TableCell>
                </TableRow>
            </TableHead>

            <TableBody>
                {approvals.map((row) => (
                    <TableRow key={row.approvalRequestId}>
                        <TableCell>{
                                    row.entityType === "News"
                                        ? "خبر"
                                        : row.entityType === "GalleryImage"
                                        ? "صورة"
                                        : row.entityType === "GalleryVideo"
                                        ? "فيديو"
                                        : row.entityType
                                }</TableCell>

                        <TableCell>{row.title}</TableCell>

                        <TableCell>{row.requestedBy}</TableCell>

                        <TableCell>
                            {new Date(
                                row.requestedAt
                            ).toLocaleString()}
                        </TableCell>

                        <TableCell>{
                                    row.status === "Pending"
                                        ? "قيد المراجعة"
                                        : row.status === "Approved"
                                        ? "معتمد"
                                        : "مرفوض"
                                }</TableCell>

                        <TableCell align="center">
                            <Stack
                                direction="row"
                                spacing={1}
                                justifyContent="center"
                            >
                                <Button
                                    variant="contained"
                                    color="success"
                                    size="small"
                                    onClick={() =>
                                        handleApprove(
                                            row.approvalRequestId
                                        )
                                    }
                                >
                                    اعتماد
                                </Button>

                                <Button
                                    variant="contained"
                                    color="error"
                                    size="small"
                                    onClick={() =>
                                        handleReject(
                                            row.approvalRequestId
                                        )
                                    }
                                >
                                    رفض
                                </Button>
                            </Stack>
                        </TableCell>
                    </TableRow>
                ))}
            </TableBody>
        </Table>
        <Dialog
        open={rejectDialogOpen}
        onClose={() => setRejectDialogOpen(false)}
        fullWidth
        maxWidth="sm"
    >
        <DialogTitle>
            رفض الطلب
        </DialogTitle>

        <DialogContent>
            <TextField
                autoFocus
                margin="dense"
                label="سبب الرفض"
                fullWidth
                multiline
                rows={4}
                value={reason}
                onChange={(e) => setReason(e.target.value)}
            />
        </DialogContent>

        <DialogActions>
            <Button
                onClick={() => setRejectDialogOpen(false)}
            >
                إلغاء
            </Button>

            <Button
                color="error"
                variant="contained"
                onClick={confirmReject}
            >
                رفض
            </Button>
        </DialogActions>
    </Dialog>
    </>
    );
}
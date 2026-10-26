import React, { useState, useEffect } from 'react';
import { useMutation } from '@tanstack/react-query';
import { Button } from '@takeoff-ui/react-spar';
import { SparDateField, SparTextField } from '../../shared/ui';
import { dataProvider } from '../../dataProvider';
import { overlayStyle, modalStyle } from '../../styles';
import { LookupSelect } from '../../shared/LookupSelect';
import { useFlows } from '../../flows/FlowProvider';

type ExpenseRequestRecord = {
  id: string | number;
  title: string;
  amount: number;
  requestDate: string;
  description?: string;
  status: string;
  revisionNote?: string;
  budgetLineId: string;
  expenseCategoryId: string;
  departmentId: string;
};

interface ExpenseRequestEditProps {
  record: ExpenseRequestRecord | null;
  onClose: () => void;
  onSuccess: () => void;
}

export const ExpenseRequestEdit: React.FC<ExpenseRequestEditProps> = ({ record, onClose, onSuccess }) => {
  const [form, setForm] = useState<Partial<ExpenseRequestRecord>>(record ?? {});
  const setField = (name: string, value: unknown) => setForm((p) => ({ ...p, [name]: value }));
  const { triggerFlows } = useFlows();

  useEffect(() => {
    if (record) setForm({ ...record });
  }, [record]);

  const mutation = useMutation({
    mutationFn: (values: Partial<ExpenseRequestRecord>) =>
      dataProvider.update('ExpenseRequest', record!.id, values),
    onSuccess: (_data, values) => { triggerFlows('update', 'ExpenseRequest', values as Record<string, unknown>); onSuccess(); onClose(); },
    onError: (err: Error) => { window.dispatchEvent(new CustomEvent('app-toast', { detail: { type: 'error', message: err.message } })); },
  });

  if (!record) return null;

  return (
    <div style={overlayStyle} onClick={onClose}>
      <div style={modalStyle} onClick={(e) => e.stopPropagation()}>
        <div style={{ padding: '20px 28px 0', flexShrink: 0, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <h2 style={{ margin: 0, fontSize: 18, fontWeight: 700 }}>Edit ExpenseRequest</h2>
          <button onClick={onClose} style={{ background: 'none', border: 'none', fontSize: 20, cursor: 'pointer', color: '#666', padding: '4px 8px', borderRadius: 4 }} onMouseOver={(e) => (e.currentTarget.style.color = '#333')} onMouseOut={(e) => (e.currentTarget.style.color = '#666')}>✕</button>
        </div>
        <form onSubmit={(e) => { e.preventDefault(); mutation.mutate(form); }} style={{ display: 'flex', flexDirection: 'column', flex: 1, overflow: 'hidden' }}>
          <div style={{ flex: 1, overflowY: 'auto', padding: '20px 28px' }}>
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(12, minmax(0, 1fr))', gap: '16px' }}>
                <div style={{ gridColumn: 'span 6' }}>
                  <SparTextField label="Title *" value={String(form.title ?? '')} onChange={(v) => setField('title', v)} />
                </div>
                <div style={{ gridColumn: 'span 6' }}>
                  <SparTextField label="Amount *" value={String(form.amount ?? '')} onChange={(v) => setField('amount', v === '' ? null : Number(v))} type="number" />
                </div>
                <div style={{ gridColumn: 'span 6' }}>
                  <SparDateField label="Request Date *" value={String(form.requestDate ?? '')} onChange={(v) => setField('requestDate', v)} withTime />
                </div>
                <div style={{ gridColumn: 'span 6' }}>
                  <SparTextField label="Description" value={String(form.description ?? '')} onChange={(v) => setField('description', v)} />
                </div>
                <div style={{ gridColumn: 'span 6' }}>
                  <LookupSelect label="Status *" value={String(form.status ?? '')} onChange={(v) => setField('status', v ? Number(v) : null)} searchable={false} options={[{ label: "Draft", value: "0" }, { label: "PendingManagerApproval", value: "1" }, { label: "PendingFinanceApproval", value: "2" }, { label: "Approved", value: "3" }, { label: "Revision", value: "4" }, { label: "Rejected", value: "5" }, { label: "Cancelled", value: "6" }]} />
                </div>
                <div style={{ gridColumn: 'span 6' }}>
                  <SparTextField label="Revision Note" value={String(form.revisionNote ?? '')} onChange={(v) => setField('revisionNote', v)} />
                </div>
                <div style={{ gridColumn: 'span 6' }}>
                  <LookupSelect label="Bütçe Kalemi *" resource="BudgetLine" value={String(form.budgetLineId ?? '')} onChange={(v) => setField('budgetLineId', v)} displayField="description" />
                </div>
                <div style={{ gridColumn: 'span 6' }}>
                  <LookupSelect label="Harcama Kategorisi *" resource="ExpenseCategory" value={String(form.expenseCategoryId ?? '')} onChange={(v) => setField('expenseCategoryId', v)} displayField="name" />
                </div>
                <div style={{ gridColumn: 'span 6' }}>
                  <LookupSelect label="Departman *" resource="Department" value={String(form.departmentId ?? '')} onChange={(v) => setField('departmentId', v)} displayField="name" />
                </div>
            </div>
          </div>
          <div style={{ display: 'flex', justifyContent: 'flex-end', gap: 8, padding: '16px 28px', borderTop: '1px solid #e8e8e8', flexShrink: 0, background: '#fff' }}>
            <Button variant="primary" appearance="outlined" onClick={onClose}>Cancel</Button>
            <Button variant="primary" appearance="filled" type="submit" disabled={mutation.isPending}>{mutation.isPending ? 'Saving…' : 'Save Changes'}</Button>
          </div>
        </form>
      </div>
    </div>
  );
};

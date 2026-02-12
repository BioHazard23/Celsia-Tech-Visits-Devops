import React, { useState } from 'react';

const AppointmentForm = () => {
    const [activeTab, setActiveTab] = useState('schedule');
    const [formData, setFormData] = useState({
        nic: '',
        name: '',
        date: '',
        timeSlot: 'AM',
    });
    const [errors, setErrors] = useState({});
    const [status, setStatus] = useState('');
    const [isSubmitting, setIsSubmitting] = useState(false);

    // Search functionality state
    const [lookupNic, setLookupNic] = useState('');
    const [appointments, setAppointments] = useState([]);
    const [lookupStatus, setLookupStatus] = useState('');

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
        // Reset validation errors on input
        if (errors[name]) {
            setErrors({ ...errors, [name]: '' });
        }
    };

    const validateForm = () => {
        const newErrors = {};

        if (!formData.nic || formData.nic.trim().length < 4) {
            newErrors.nic = 'NIC must be at least 4 characters';
        }
        if (!/^\d+$/.test(formData.nic)) {
            newErrors.nic = 'NIC must contain only numbers';
        }
        if (!formData.name || formData.name.trim().length < 2) {
            newErrors.name = 'Full name is required (min. 2 characters)';
        }
        if (!formData.date) {
            newErrors.date = 'Please select a date';
        } else {
            const selected = new Date(formData.date + 'T00:00:00');
            const today = new Date();
            today.setHours(0, 0, 0, 0);
            if (selected < today) {
                newErrors.date = 'Date cannot be in the past';
            }
            const day = selected.getDay();
            if (day === 0 || day === 6) {
                newErrors.date = 'Weekends are not available for visits';
            }
        }

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!validateForm()) return;

        setIsSubmitting(true);
        setStatus('loading');

        try {
            const payload = {
                date: formData.date,
                timeSlot: formData.timeSlot,
                status: 'Scheduled',
                customer: {
                    nic: formData.nic,
                    name: formData.name
                }
            };

            const response = await fetch('http://localhost:5157/api/appointments', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload),
            });

            if (response.ok) {
                setStatus('success');
                setFormData({ nic: '', name: '', date: '', timeSlot: 'AM' });
                setErrors({});
            } else {
                const errorText = await response.text();
                console.error('Server error:', errorText);
                setStatus('error');
            }
        } catch (error) {
            console.error('Error:', error);
            setStatus('error');
        } finally {
            setIsSubmitting(false);
        }
    };

    const handleLookup = async (e) => {
        e.preventDefault();
        if (!lookupNic || lookupNic.trim().length < 4) {
            setLookupStatus('invalid');
            return;
        }

        setLookupStatus('loading');
        try {
            const response = await fetch(`http://localhost:5157/api/appointments/nic/${lookupNic}`);
            if (response.ok) {
                const data = await response.json();
                setAppointments(data);
                setLookupStatus(data.length > 0 ? 'found' : 'empty');
            } else {
                setLookupStatus('error');
            }
        } catch (error) {
            console.error('Lookup error:', error);
            setLookupStatus('error');
        }
    };

    const formatDate = (dateStr) => {
        const date = new Date(dateStr);
        return date.toLocaleDateString('es-CO', {
            weekday: 'short',
            year: 'numeric',
            month: 'short',
            day: 'numeric'
        });
    };

    const getStatusBadge = (status) => {
        const styles = {
            Scheduled: { bg: '#FFF3EB', color: '#C74E10', label: '📅 Programada' },
            Completed: { bg: '#ECFDF5', color: '#065F46', label: '✅ Completada' },
            Cancelled: { bg: '#FEF2F2', color: '#991B1B', label: '❌ Cancelada' },
        };
        const s = styles[status] || styles.Scheduled;
        return (
            <span style={{
                background: s.bg,
                color: s.color,
                padding: '4px 10px',
                borderRadius: '8px',
                fontSize: '12px',
                fontWeight: 600
            }}>
                {s.label}
            </span>
        );
    };

    const getStatusMessage = () => {
        switch (status) {
            case 'loading':
                return <div className="celsia-status-loading">⏳ Scheduling your appointment...</div>;
            case 'success':
                return <div className="celsia-status-success">✅ Appointment scheduled successfully! We'll see you soon.</div>;
            case 'error':
                return <div className="celsia-status-error">❌ Could not schedule the appointment. Please try again.</div>;
            default:
                return null;
        }
    };

    return (
        <div className="celsia-card p-8">
            {/* Tabs */}
            <div style={{ display: 'flex', gap: '4px', marginBottom: '24px', background: '#F3F4F6', borderRadius: '12px', padding: '4px' }}>
                <button
                    type="button"
                    onClick={() => setActiveTab('schedule')}
                    className="celsia-tab"
                    style={{
                        flex: 1,
                        padding: '10px 16px',
                        borderRadius: '10px',
                        border: 'none',
                        fontSize: '14px',
                        fontWeight: 600,
                        cursor: 'pointer',
                        transition: 'all 0.2s ease',
                        background: activeTab === 'schedule' ? '#FFFFFF' : 'transparent',
                        color: activeTab === 'schedule' ? '#E8611A' : '#6B7280',
                        boxShadow: activeTab === 'schedule' ? '0 1px 3px rgba(0,0,0,0.1)' : 'none',
                    }}
                >
                    📝 Schedule Visit
                </button>
                <button
                    type="button"
                    onClick={() => setActiveTab('lookup')}
                    className="celsia-tab"
                    style={{
                        flex: 1,
                        padding: '10px 16px',
                        borderRadius: '10px',
                        border: 'none',
                        fontSize: '14px',
                        fontWeight: 600,
                        cursor: 'pointer',
                        transition: 'all 0.2s ease',
                        background: activeTab === 'lookup' ? '#FFFFFF' : 'transparent',
                        color: activeTab === 'lookup' ? '#E8611A' : '#6B7280',
                        boxShadow: activeTab === 'lookup' ? '0 1px 3px rgba(0,0,0,0.1)' : 'none',
                    }}
                >
                    🔍 My Visits
                </button>
            </div>

            {/* Schedule Tab */}
            {activeTab === 'schedule' && (
                <>
                    <h2 className="text-xl font-bold mb-1" style={{ color: '#1A1A2E' }}>
                        Schedule Your Visit
                    </h2>
                    <p className="text-sm mb-6" style={{ color: '#9CA3AF' }}>
                        Fill in the details below to book a technical visit
                    </p>

                    <form onSubmit={handleSubmit} className="space-y-5" noValidate>
                        {/* Customer ID (NIC) */}
                        <div>
                            <label className="celsia-label" htmlFor="nic">NIC (Customer ID)</label>
                            <input
                                id="nic"
                                type="text"
                                name="nic"
                                value={formData.nic}
                                onChange={handleChange}
                                className={`celsia-input ${errors.nic ? 'celsia-input-error' : ''}`}
                                placeholder="e.g. 123456"
                                inputMode="numeric"
                            />
                            {errors.nic && <p className="celsia-field-error">{errors.nic}</p>}
                        </div>

                        {/* Full Name */}
                        <div>
                            <label className="celsia-label" htmlFor="name">Full Name</label>
                            <input
                                id="name"
                                type="text"
                                name="name"
                                value={formData.name}
                                onChange={handleChange}
                                className={`celsia-input ${errors.name ? 'celsia-input-error' : ''}`}
                                placeholder="Enter your full name"
                            />
                            {errors.name && <p className="celsia-field-error">{errors.name}</p>}
                        </div>

                        {/* Appointment Date */}
                        <div>
                            <label className="celsia-label" htmlFor="date">Preferred Date</label>
                            <input
                                id="date"
                                type="date"
                                name="date"
                                value={formData.date}
                                onChange={handleChange}
                                className={`celsia-input ${errors.date ? 'celsia-input-error' : ''}`}
                                min={new Date().toISOString().split('T')[0]}
                            />
                            {errors.date && <p className="celsia-field-error">{errors.date}</p>}
                        </div>

                        {/* Time Slot */}
                        <div>
                            <label className="celsia-label" htmlFor="timeSlot">Time Slot</label>
                            <select
                                id="timeSlot"
                                name="timeSlot"
                                value={formData.timeSlot}
                                onChange={handleChange}
                                className="celsia-select"
                            >
                                <option value="AM">🌅 Morning (8:00 AM – 12:00 PM)</option>
                                <option value="PM">🌇 Afternoon (1:00 PM – 5:00 PM)</option>
                            </select>
                        </div>

                        {/* Submit */}
                        <button
                            type="submit"
                            className="celsia-btn celsia-btn-primary"
                            disabled={isSubmitting}
                        >
                            {isSubmitting ? 'Scheduling...' : '⚡ Confirm Appointment'}
                        </button>

                        {status && <div className="mt-2">{getStatusMessage()}</div>}
                    </form>
                </>
            )}

            {/* Lookup Tab */}
            {activeTab === 'lookup' && (
                <>
                    <h2 className="text-xl font-bold mb-1" style={{ color: '#1A1A2E' }}>
                        My Scheduled Visits
                    </h2>
                    <p className="text-sm mb-6" style={{ color: '#9CA3AF' }}>
                        Enter your NIC to view your appointments
                    </p>

                    <form onSubmit={handleLookup} className="space-y-4">
                        <div style={{ display: 'flex', gap: '8px' }}>
                            <input
                                type="text"
                                value={lookupNic}
                                onChange={(e) => { setLookupNic(e.target.value); setLookupStatus(''); }}
                                className="celsia-input"
                                placeholder="Enter your NIC"
                                inputMode="numeric"
                                style={{ flex: 1 }}
                            />
                            <button
                                type="submit"
                                className="celsia-btn celsia-btn-primary"
                                style={{ width: 'auto', padding: '14px 24px' }}
                            >
                                Search
                            </button>
                        </div>
                        {lookupStatus === 'invalid' && (
                            <p className="celsia-field-error">Please enter at least 4 digits</p>
                        )}
                    </form>

                    {/* Results */}
                    {lookupStatus === 'loading' && (
                        <div className="celsia-status-loading mt-4">⏳ Searching for appointments...</div>
                    )}
                    {lookupStatus === 'empty' && (
                        <div className="celsia-status-loading mt-4">📭 No appointments found for this NIC.</div>
                    )}
                    {lookupStatus === 'error' && (
                        <div className="celsia-status-error mt-4">❌ Error searching for appointments.</div>
                    )}
                    {lookupStatus === 'found' && appointments.length > 0 && (
                        <div className="mt-6 space-y-3">
                            <p style={{ fontSize: '13px', color: '#6B7280', fontWeight: 600 }}>
                                {appointments.length} appointment{appointments.length > 1 ? 's' : ''} found for{' '}
                                <span style={{ color: '#E8611A' }}>{appointments[0]?.customer?.name || lookupNic}</span>
                            </p>
                            {appointments.map((apt) => (
                                <div
                                    key={apt.id}
                                    style={{
                                        padding: '16px',
                                        borderRadius: '12px',
                                        border: '1px solid #E5E7EB',
                                        background: '#FAFAFA',
                                        transition: 'all 0.2s ease',
                                    }}
                                >
                                    <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '8px' }}>
                                        <span style={{ fontSize: '15px', fontWeight: 600, color: '#1A1A2E' }}>
                                            {formatDate(apt.date)}
                                        </span>
                                        {getStatusBadge(apt.status)}
                                    </div>
                                    <div style={{ display: 'flex', gap: '16px', fontSize: '13px', color: '#6B7280' }}>
                                        <span>{apt.timeSlot === 'AM' ? '🌅 Morning' : '🌇 Afternoon'}</span>
                                        <span>ID: #{apt.id}</span>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </>
            )}
        </div>
    );
};

export default AppointmentForm;

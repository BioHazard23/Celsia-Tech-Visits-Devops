import AppointmentForm from './components/AppointmentForm';

function App() {
  return (
    <div className="min-h-screen flex flex-col items-center justify-center p-4 relative">
      {/* Animated Background */}
      <div className="celsia-bg-pattern" />

      {/* Main Content */}
      <div className="relative z-10 w-full max-w-lg">
        {/* Header */}
        <header className="text-center mb-8">
          <img
            src="/celsia-logo.png"
            alt="Celsia"
            className="celsia-logo mx-auto mb-4"
          />
          <h1 className="text-3xl font-extrabold tracking-tight"
            style={{ color: '#1A1A2E' }}>
            Tech Visits
          </h1>
          <p className="mt-2 text-sm" style={{ color: '#4A4A5A' }}>
            Schedule your technical service appointment quickly and easily
          </p>
          <div className="celsia-divider mt-4" />
        </header>

        {/* Form Card */}
        <main>
          <AppointmentForm />
        </main>

        {/* Footer */}
        <footer className="mt-8 text-center text-xs" style={{ color: '#9CA3AF' }}>
          <p>&copy; {new Date().getFullYear()} Celsia S.A. E.S.P. &mdash; All rights reserved.</p>
          <p className="mt-1">Fast and reliable technical service scheduling</p>
        </footer>
      </div>
    </div>
  );
}

export default App;

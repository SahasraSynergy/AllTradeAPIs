import logo from './logo.svg';
import './App.css';
import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import AnnouncementsList from "./AnnouncementsList";
import AddAnnouncement from "./AddAnnouncement";
function App() {
  return (
    <div className="App">
       <header className="App-header">
        <img src={logo} className="App-logo" alt="Custom Logo" />
      </header>
          <Router>
            <Routes>
              <Route path="/" element={<AnnouncementsList />} />
              <Route path="/add-announcement" element={<AddAnnouncement />} />
            </Routes>
          </Router>
    </div>
  );
}

export default App;

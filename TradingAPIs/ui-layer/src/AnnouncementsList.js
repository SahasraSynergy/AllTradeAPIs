import React, { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import config from "./config"; // Import the base URL
import "bootstrap/dist/css/bootstrap.min.css"; // Import Bootstrap CSS

const AnnouncementsList = () => {
    const [announcements, setAnnouncements] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        axios
            .get(`${config.API_BASE_URL}/Announcements`, {
                headers: {
                    "Content-Type": "application/json",
                },
                withCredentials: true, // If you need to include credentials like cookies or authentication tokens
            })
            .then((response) => setAnnouncements(response.data))
            .catch((error) => console.error("Error fetching announcements:", error));
    }, []);

    return (
        <div className="container mt-5">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h1 className="text-primary">Announcements</h1>
                <button 
                    className="btn btn-success" 
                    onClick={() => navigate("/add-announcement")}
                >
                    Add New Announcement
                </button>
            </div>
            <div className="table-responsive">
                <table className="table table-striped table-bordered">
                    <thead className="table-dark">
                        <tr>
                            <th>Title</th>
                            <th>Company Name</th>
                            <th>Category</th>
                            <th>Subcategory</th>
                            <th>Announcement Date</th>
                        </tr>
                    </thead>
                    <tbody>
                        {announcements.length > 0 ? (
                            announcements.map((announcement) => (
                                <tr key={announcement.id}>
                                    <td>{announcement.title}</td>
                                    <td>{announcement.companyName}</td>
                                    <td>{announcement.catagory}</td>
                                    <td>{announcement.subCatagory}</td>
                                    <td>
                                        {new Date(announcement.announementDate).toLocaleDateString()}
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr>
                                <td colSpan="5" className="text-center">
                                    No announcements available.
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default AnnouncementsList;

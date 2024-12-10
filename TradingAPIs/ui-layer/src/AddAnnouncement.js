import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import config from "./config"; // Import the base URL
import "bootstrap/dist/css/bootstrap.min.css"; // Import Bootstrap CSS

const AddAnnouncement = () => {
    const [formData, setFormData] = useState({
        title: "",
        companyName: "",
        catagory: "",
        subCatagory: "",
        xbrlLink: "",
        pdfLink: "",
        announcementDetails: "",
        announementDate: "",
    });

    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        axios
            .post(`${config.API_BASE_URL}/Announcements`,
                { ...formData, announementCreatedDate: new Date() },
                {
                    headers: {
                        "Content-Type": "application/json",
                    },
                    withCredentials: true, // If you need credentials or cookies
                })
            .then(() => {
                alert("Announcement added successfully!");
                navigate("/");
            })
            .catch((error) => console.error("Error adding announcement:", error));
    };

    return (
        <div className="container mt-5">
            <h1 className="text-center mb-4">Add New Announcement</h1>
            <form onSubmit={handleSubmit} className="shadow p-4 bg-light rounded">
                <div className="row mb-3">
                    <div className="col-md-6">
                        <label className="form-label">Title:</label>
                        <input
                            type="text"
                            name="title"
                            value={formData.title}
                            onChange={handleChange}
                            className="form-control form-control-sm"
                            required
                        />
                    </div>
                    <div className="col-md-6">
                        <label className="form-label">Company Name:</label>
                        <input
                            type="text"
                            name="companyName"
                            value={formData.companyName}
                            onChange={handleChange}
                            className="form-control form-control-sm"
                            required
                        />
                    </div>
                </div>
                <div className="row mb-3">
                    <div className="col-md-6">
                        <label className="form-label">Category:</label>
                        <input
                            type="text"
                            name="catagory"
                            value={formData.catagory}
                            onChange={handleChange}
                            className="form-control form-control-sm"
                            required
                        />
                    </div>
                    <div className="col-md-6">
                        <label className="form-label">Subcategory:</label>
                        <input
                            type="text"
                            name="subCatagory"
                            value={formData.subCatagory}
                            onChange={handleChange}
                            className="form-control form-control-sm"
                        />
                    </div>
                </div>
                <div className="row mb-3">
                    <div className="col-md-6">
                        <label className="form-label">XBRL Link:</label>
                        <input
                            type="url"
                            name="xbrlLink"
                            value={formData.xbrlLink}
                            onChange={handleChange}
                            className="form-control form-control-sm"
                        />
                    </div>
                    <div className="col-md-6">
                        <label className="form-label">PDF Link:</label>
                        <input
                            type="url"
                            name="pdfLink"
                            value={formData.pdfLink}
                            onChange={handleChange}
                            className="form-control form-control-sm"
                        />
                    </div>
                </div>
                <div className="mb-3">
                    <label className="form-label">Announcement Details:</label>
                    <textarea
                        name="announcementDetails"
                        value={formData.announcementDetails}
                        onChange={handleChange}
                        className="form-control form-control-sm"
                        rows="3"
                    />
                </div>
               
                <div className="row mb-3">
                <div className="col-md-6">
                    <label className="form-label">Divident Per Share:</label>
                    <textarea
                        name="DividentPerShare"
                        value={formData.DividentPerShare}
                        onChange={handleChange}
                        className="form-control form-control-sm"
                        rows="3"
                    />
                </div>
                <div className="col-md-6">
                    <label className="form-label">Divident Record Date:</label>
                    <input
                        type="date"
                        name="DividentRecordDate"
                        value={formData.dividentRecordDate}
                        onChange={handleChange}
                        className="form-control form-control-sm"
                    />
                </div>
                <div className="col-md-4">
                    <label className="form-label">Announcement Date:</label>
                    <input
                        type="date"
                        name="announementDate"
                        value={formData.announementDate}
                        onChange={handleChange}
                        className="form-control form-control-sm"
                    />
                </div>
            </div>

                <div className="text-center">
                    <button type="submit" className="btn btn-primary">Submit</button>
                </div>
            </form>
        </div>
    );
};

export default AddAnnouncement;

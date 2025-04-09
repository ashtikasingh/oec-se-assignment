import React, { useState, useEffect } from "react";
import ReactSelect from "react-select";
import { getAssignedUsers, assignUserToProcedurePlan, removeAssignedUserFromProcedurePlan, removeAllAssignedUserFromProcedurePlan } from "../../../api/api";

const PlanProcedureItem = ({ procedure, users, planId }) => {
    const [selectedUsers, setSelectedUsers] = useState([]);

    const handleAssignUserToProcedure = async (newUsers) => {
        try {
            if (newUsers.length === 0) {
                await removeAllAssignedUserFromProcedurePlan(planId, procedure.procedureId);
            } else if (newUsers.length > selectedUsers.length) {
                const addedUser = newUsers.find(user => !selectedUsers.some(u => u.value === user.value));
                if (addedUser) {
                    await assignUserToProcedurePlan(addedUser.value, planId, procedure.procedureId);
                }
            } else {
                const removedUser = selectedUsers.find(user => !newUsers.some(u => u.value === user.value));
                if (removedUser) {
                    await removeAssignedUserFromProcedurePlan(removedUser.value, planId, procedure.procedureId);
                }
            }

            setSelectedUsers(newUsers);
        } catch (error) {
            console.error("Unexpected error while assigning users:", error);
        }
    };

    useEffect(() => {
        const fetchAssignedUsers = async () => {
            try {
                const data = await getAssignedUsers(planId, procedure.procedureId);
                const userOptions = data.map(d => ({
                    label: d.user.name,
                    value: d.user.userId
                }));
                setSelectedUsers(userOptions);
            } catch (error) {
                console.error("Error fetching assigned users:", error);
            }
        };

        fetchAssignedUsers();
    }, []);


    return (
        <div className="py-2">
            <div>
                {procedure.procedureTitle}
            </div>

            <ReactSelect
                className="mt-2"
                placeholder="Select User to Assign"
                isMulti={true}
                options={users}
                value={selectedUsers}
                onChange={(e) => handleAssignUserToProcedure(e)}
            />
        </div>
    );
};

export default PlanProcedureItem;
